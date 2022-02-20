import requests
import os
import schedule
import time

url = os.environ['POWER_INFO_URL']
limit = float(os.environ['POWER_WARNING_LIMIT'])
#start_minute = os.environ['POWER_WARNING_START_MINUTE']


def investigate_and_report():
    data = requests.get(url).json()
    prognosis = data['hourPrognosis']
    if prognosis > limit:
        # beep!
        print("sending alarm")
        requests.get("http://192.168.50.80/beep")
    
#schedule.every(2).seconds.do(job)

#while True:
#    schedule.run_pending()
#    time.sleep(2)