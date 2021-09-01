import datetime
import os 
import sys
import pandas
from prophet import Prophet
import prophet
from models.settings import Settings
from utils import utils

from models.datasources.prometheus_datasource import PrometheusDatasource
from models.datasources.datasources import Datasources

# env variables

# query = utils.getenv_or_raise("QUERY")
# datasource_url = utils.getenv_or_raise("DATASOURCE_URL")
# datasource_type = utils.getenv_or_raise("DATASOURCE_TYPE")
# datasource_is_authenticated = os.getenv("DATASOURCE_IS_AUTHENTICATED", False)

# if datasource_is_authenticated: 
#     datasource_username = utils.getenv_or_raise("DATASOURCE_USERNAME")
#     datasource_password = utils.getenv_or_raise("DATASOURCE_PASSWORD")


# steps:
# load data from the datasource with the query
# load 

# data = datasource.query_data(timerange)

# model = Prophet(
#     daily_seasonality=True, \
#     weekly_seasonality=True, \
#     yearly_seasonality=True
# )

# model.fit(data)

# future_time = model.make_future_dataframe(
#     periods=int(prediction_duration),
#     freq=prediction_freq,
#     include_history=False,
# )

# forecast = model.predict(future_time)

# datasource.save_data(forecast)

def train():

    settings = Settings.load_from_env()

    datasource = Datasources.get_datasource_implementation_from_settings(settings)

    # datasource = PrometheusDatasource(
    #     url="http://192.168.1.102:9090",
    #     is_authenticated=False
    # )

    # query = """rate(loki_distributor_lines_received_total[5m])""" #"up"
    settings.query = """sum(avg_over_time(kube_metrics_server_pods_cpu[120m])) / 1e6"""
    start_date = datetime.datetime.utcnow() - datetime.timedelta(days=7)
    end_date = datetime.datetime.utcnow()

    df = datasource.query_data(settings.query, start_date, end_date)
    
    # df.ds = pd.to_datetime(df.ds)

    model = prophet.Prophet(
        daily_seasonality=False, \
        weekly_seasonality=False, \
        yearly_seasonality=False
    )
    model.add_seasonality(name='daily', period=1, fourier_order=30)

    model.fit(df)

    future_time = model.make_future_dataframe(
        periods=int(48),
        freq="30MIN",
        include_history=True,
    )

    forecast = model.predict(future_time)




if __name__ == "__main__":
    pass