from datetime import datetime, timedelta
from utils import power_aggregator

now = datetime.utcnow()
start = datetime.utcnow() - timedelta(days=360)
start = start.replace(hour=0, minute=0, second=0, microsecond=0)

while start < datetime.utcnow():
    print(start)
    stop = start + timedelta(hours=1)
    power_aggregator.write_to_db(start, stop)
    start += timedelta(hours=1)
