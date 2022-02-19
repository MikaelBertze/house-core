FROM python:3
RUN apt update && apt install tzdata -y
ENV TZ="Europe/Stockholm"
WORKDIR /periodic_workers
COPY ./app .
#COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt
COPY . .

ENTRYPOINT ["python"]
CMD [ "./worker.py" ]