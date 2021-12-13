from datetime import datetime, timedelta
from pymongo import MongoClient
import numpy as np
import os

db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

cnx = MongoClient(db_host)
db = cnx[db_name]


def aggregate(start_date, stop_date):

    data = list(db.power.find({'ts': {'$gt': start_date, '$lt': stop_date}}))
    powerdata = [((d['ts'] - start_date).total_seconds(), d['mean_effect']) for d in data]
    if len(powerdata) == 0:
        return 0
    x, y = [[i for i, j in powerdata], [j for i, j in powerdata]]

    x[0] = 0
    x[-1] = (stop_date - start_date).total_seconds()

    usage = np.trapz(y, x) / 3600 / 1000

    return usage


def write_to_db(start_utc: datetime, stop_utc: datetime):
    agg = aggregate(start_utc, stop_utc)
    query = {'start': start_utc, 'stop': stop_utc}
    update = {'$set': {'start': start_utc,
                       'stop': stop_utc,
                       'duration_s': (stop_utc-start_utc).total_seconds(),
                       'consumption': agg,
                       'unit': 'kWh'
                       }
              }

    print(update)
    db.power_per_hour.update_one(query, update, upsert=True)



