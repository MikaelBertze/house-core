from datetime import datetime, timedelta
from utils import power_aggregator

now = datetime.utcnow()
start = now.replace(minute=0, second=0, microsecond=0)

power_aggregator.write_to_db(start, now)
