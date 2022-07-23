from datetime import datetime, timedelta
from pymongo import MongoClient
import numpy as np
import os

db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

cnx = MongoClient(db_host)
db = cnx[db_name]


def aggregate(start_date, stop_date):
    assert stop_date > start_date, "oups! cant stop before start..."
    data = list(db.power.find({'ts': {'$gt': start_date, '$lt': stop_date}}))
    powerdata = [((d['ts'] - start_date).total_seconds(), d['mean_effect']) for d in data]
    if len(powerdata) == 0:
        return 0
    
    usage = _calculate_usage(powerdata)
    # x, y = [[i for i, j in powerdata], [j for i, j in powerdata]]

    # x[0] = 0
    # x[-1] = (stop_date - start_date).total_seconds()

    # usage = np.trapz(y, x) / 3600 / 1000
    # print(usage)
    return usage

def aggreagate_shelly(start_date, stop_date):
    for i in range(2):
        data = list(db.shelly.find({'ts': {'$gt': start_date, '$lt': stop_date}, 'switch': i}))
        powerdata = [((d['ts'] - start_date).total_seconds(), d['mean_effect']) for d in data]
        if len(powerdata) == 0:
            yield (i, 0)
            continue

        usage = _calculate_usage(powerdata)
        yield (i, usage)
    

    
    
    
    

def write_to_db(start_utc: datetime, stop_utc: datetime):
    agg = aggregate(start_utc, stop_utc)


    query = {'start': start_utc}
    update = {'$set': {'start': start_utc,
                       'stop': stop_utc,
                       'duration_s': (stop_utc-start_utc).total_seconds(),
                       'consumption': agg,
                       'unit': 'kWh',
                       'shelly': [{'switch': i, 'consumption': c} for i,c in aggreagate_shelly(start_utc, stop_utc)]
                      }
              }
    db.power_per_hour.update_one(query, update, upsert=True)

def _calculate_usage(data):
    x, y = [[i for i, j in data], [j for i, j in data]]
    x[0] = 0
    #x[-1] = (stop_date - start_date).total_seconds()

    usage = np.trapz(y, x) / 3600 / 1000

    return usage    

