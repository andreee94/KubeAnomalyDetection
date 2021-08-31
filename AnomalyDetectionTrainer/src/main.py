import os 
import sys
import pandas
from prophet import Prophet
from utils import utils


# env variables

query = utils.getenv_or_raise("QUERY")
datasource_url = utils.getenv_or_raise("DATASOURCE_URL")
datasource_type = utils.getenv_or_raise("DATASOURCE_TYPE")
datasource_is_authenticated = os.getenv("DATASOURCE_IS_AUTHENTICATED", False)

if datasource_is_authenticated: 
    datasource_username = utils.getenv_or_raise("DATASOURCE_USERNAME")
    datasource_password = utils.getenv_or_raise("DATASOURCE_PASSWORD")



# steps:
# load data from the datasource with the query
# load 

data = datasource.query_data(timerange)

model = Prophet(
    daily_seasonality=True, \
    weekly_seasonality=True, \
    yearly_seasonality=True
)

model.fit(data)

future_time = model.make_future_dataframe(
    periods=int(prediction_duration),
    freq=prediction_freq,
    include_history=False,
)

forecast = model.predict(future_time)

datasource.save_data(forecast)