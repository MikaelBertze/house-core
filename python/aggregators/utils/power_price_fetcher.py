from datetime import datetime, timedelta
import requests
from dateutil import parser
from pymongo import MongoClient
import os

db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

cnx = MongoClient(db_host)
db = cnx[db_name]

def fetch():
    start = datetime.now() - timedelta(days=3)
    stop = start + timedelta(days=5)

    from_date = start.strftime("%Y-%m-%d")
    to_date = stop.strftime("%Y-%m-%d")

    url = f"https://www.vattenfall.se/api/price/spot/pricearea/{from_date}/{to_date}/SN3"

    print(url)

    data = requests.get(url).json()

    for d in data:
        write_to_db(d)
        
def write_to_db(data):
    print(data)
    #dt = parser.parse(data['TimeStamp'])
    query = {'TimeStamp': data['TimeStamp'], 'PriceArea': data['PriceArea']}
    update = {'$set': data }
    print(update)
    db.power_price_hour.update_one(query, update, upsert=True)

