from datetime import datetime, timedelta
from pymongo import MongoClient
import os

db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

cnx = MongoClient(db_host)
db = cnx[db_name]


def aggregate(start_date, stop_date):

    data = list(db.water.find({'ts': {'$gt': start_date, '$lt': stop_date}}))
    waterdata = [d['consumption'] for d in data]

    if len(waterdata) == 0:
        return 0
    return sum(waterdata)    

def write_to_db(start_utc: datetime, stop_utc: datetime):
    agg = aggregate(start_utc, stop_utc)
    query = {'start': start_utc}
    update = {'$set': {'start': start_utc,
                       'stop': stop_utc,
                       'duration_s': (stop_utc-start_utc).total_seconds(),
                       'consumption': agg,
                       'unit': 'Liter'
                       }
              }

    print(update)
    db.water_per_hour.update_one(query, update, upsert=True)



