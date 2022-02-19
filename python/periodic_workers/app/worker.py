import os
import time
import logging
from datetime import datetime, timedelta
from aggregators import power_aggregator
from aggregators import power_price_fetcher
from aggregators import water_aggregator
from misc import power_warning
import schedule


if os.environ.get("DEBUG") == "1":
    logging.getLogger().setLevel(logging.DEBUG)

def aggregate_power():
    try:
        logging.info("Aggregate power")
        start = datetime.utcnow().replace(minute=0, second=0, microsecond=0)
        end = datetime.utcnow()
        power_aggregator.write_to_db(start, end)
    except Exception as ex:
        logging.exception(ex)

def aggregate_power_previous_hour():
    try:
        logging.info("Aggregate power previous hour")
        end = datetime.utcnow().replace(minute=0, second=0, microsecond=0)
        start = end - timedelta(hours=1)
        power_aggregator.write_to_db(start, end)
    except Exception as ex:
        logging.exception(ex)

def aggregate_water():
    try:
        logging.info("Aggregate water")
        start = datetime.utcnow().replace(minute=0, second=0, microsecond=0)
        end = datetime.utcnow()
        water_aggregator.write_to_db(start, end)
    except Exception as ex:
        logging.exception(ex)

def aggregate_water_previous_hour():
    try:
        logging.info("Aggregate water previous hour")
        end = datetime.utcnow().replace(minute=0, second=0, microsecond=0)
        start = end - timedelta(hours=1)
        water_aggregator.write_to_db(start, end)
    except Exception as ex:
        logging.exception(ex)

def over_usage_warning():
    try:
        # only run if 15 minutes in
        if datetime.now().minute > 15:
            logging.info("power warning")
            power_warning.investigate_and_report()
    except Exception as ex:
        logging.exception(ex)

def fetch_power_price():
    try:
        logging.info("fetching power price")
        power_price_fetcher.fetch()
    except Exception as ex:
        logging.exception(ex)

schedule.every().minute.at(":00").do(aggregate_power)
schedule.every().minute.at(":00").do(aggregate_water)
schedule.every().hour.at("00:05").do(aggregate_power_previous_hour) # 5s in to avoid risk of execution at 59:59
schedule.every().hour.at("00:05").do(aggregate_water_previous_hour) # 5s in to avoid risk of execution at 59:59
schedule.every().minute.at(":00").do(over_usage_warning)

schedule.every().hour.do(fetch_power_price)

# create schedule
schedule.run_all()

while True:
    schedule.run_pending()
    time.sleep(1)
