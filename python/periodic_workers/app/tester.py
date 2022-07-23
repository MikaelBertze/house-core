import os
#import time
#import logging
from datetime import datetime, timedelta

#from aggregators import water_aggregator
#from app import aggregators
#from misc import power_warning

# from aggregators import power_aggregator
# print(power_aggregator.db_name)
# start = datetime.utcnow().replace(minute=0, second=0, microsecond=0)
# end = datetime.utcnow()
# power_aggregator.write_to_db(start, end)

from aggregators import shelly_aggregator
print(shelly_aggregator.db_name)
start = (datetime.utcnow()).replace(minute=0, second=0, microsecond=0)
end = datetime.utcnow() # start + timedelta(hours=1)

print(start)
print(end)
shelly_aggregator.write_to_db(start, end)


# from aggregators import power_price_fetcher
# power_price_fetcher.fetch()

#from misc import shelly_trigger
#shelly_trigger.investigate_and_set()
