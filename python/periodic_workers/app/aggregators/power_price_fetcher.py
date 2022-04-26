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

def getData(dt):
    print("Fetching")
    print(dt)
    try:
        date = dt.strftime("%d-%m-%Y")
        response = requests.get(f"https://www.nordpoolgroup.com/api/marketdata/page/10?currency=SEK,SEK,EUR,EUR&endDate={date}")
        dd = response.json()

        data = []
        for row in [x for x in dd['data']['Rows'] if not x['IsExtraRow']]:
            dt = parser.parse(row['StartTime']).replace(tzinfo=tz.gettz("Europe/Stockholm"))
            #print(dt)
            price = next((x for x in row['Columns'] if x['Name']=="SE3"))['Value']
            price = round(float(price.replace(',', '.').replace(' ', ''))/10,2)
            #print(price)

            dt_day = dt.strftime("%Y-%m-%d")
            #print(dt_day)
            dt_hour = dt.strftime("%H:%M")
            #print(dt_hour)
            x = {'TimeStamp': row['StartTime'], 'TimeStampDay':dt_day, 'TimeStampHour': dt_hour, 'Value': price, 'PriceArea': 'SN3', "ts": dt, 'source': "nordpool"}
            data.append(x)

        

        return data
    except:
        print("Error")
        return []

def fetch():
    dt = datetime.now() # - timedelta(days=13)
    data = getData(dt)
    if dt.hour > 15:
        dt = dt + timedelta(days=1)
        data = data + getData(dt)


    #from_date = start.strftime("%Y-%m-%d")
    #to_date = stop.strftime("%Y-%m-%d")

    #url = f"https://www.vattenfall.se/api/price/spot/pricearea/{from_date}/{to_date}/SN3"
    #data = requests.get(url).json()

    for d in data:
        write_to_db(d)
        
def write_to_db(data):
    #dt = parser.parse(data['TimeStamp']).replace(tzinfo=tz.gettz("Europe/Stockholm"))
    #data['ts'] = dt
    query = {'TimeStamp': data['TimeStamp'], 'PriceArea': data['PriceArea']}
    update = {'$set': data }
    db.power_price_hour.update_one(query, update, upsert=True)



# Vattenfall

# def fetch():
#     start = datetime.now() - timedelta(days=13)
#     stop = start + timedelta(days=25)

#     from_date = start.strftime("%Y-%m-%d")
#     to_date = stop.strftime("%Y-%m-%d")

#     url = f"https://www.vattenfall.se/api/price/spot/pricearea/{from_date}/{to_date}/SN3"
#     data = requests.get(url).json()

#     for d in data:
#         write_to_db(d)
        
# def write_to_db(data):
#     dt = parser.parse(data['TimeStamp']).replace(tzinfo=tz.gettz("Europe/Stockholm"))
#     data['ts'] = dt
#     query = {'TimeStamp': data['TimeStamp'], 'PriceArea': data['PriceArea']}
#     update = {'$set': data }
#     db.power_price_hour.update_one(query, update, upsert=True)

