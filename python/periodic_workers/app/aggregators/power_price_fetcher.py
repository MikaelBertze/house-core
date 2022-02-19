from datetime import datetime, timedelta, timezone
import dateutil
import requests
from dateutil import tz, parser
from pymongo import MongoClient
import os

db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

cnx = MongoClient(db_host)
db = cnx[db_name]

def fetch():
    start = datetime.now() - timedelta(days=13)
    stop = start + timedelta(days=25)

    from_date = start.strftime("%Y-%m-%d")
    to_date = stop.strftime("%Y-%m-%d")

    url = f"https://www.vattenfall.se/api/price/spot/pricearea/{from_date}/{to_date}/SN3"
    data = requests.get(url).json()

    for d in data:
        write_to_db(d)
        
def write_to_db(data):
    dt = parser.parse(data['TimeStamp']).replace(tzinfo=tz.gettz("Europe/Stockholm"))
    data['ts'] = dt
    query = {'TimeStamp': data['TimeStamp'], 'PriceArea': data['PriceArea']}
    update = {'$set': data }
    db.power_price_hour.update_one(query, update, upsert=True)

