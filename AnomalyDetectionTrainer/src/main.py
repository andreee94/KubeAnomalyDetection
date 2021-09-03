import datetime
import prophet
from models.settings.settings import Settings
from models.datasources.datasources import Datasources
from models.storages.storages import Storages

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

    if settings.debug:
        from ui.debug_ui import DebugUI
        debugUI = DebugUI()
        debugUI.setup()

    datasource = Datasources.get_datasource_implementation_from_settings(settings.datasource)
    storage = Storages.get_storage_implementation_from_settings(settings.storage)

    # datasource = PrometheusDatasource(
    #     url="http://192.168.1.102:9090",
    #     is_authenticated=False
    # )

    # query = """rate(loki_distributor_lines_received_total[5m])""" #"up"
    settings.query = """sum(avg_over_time(kube_metrics_server_pods_cpu[120m])) / 1e6"""
    start_date = datetime.datetime.utcnow() - datetime.timedelta(days=1)
    end_date = datetime.datetime.utcnow()

    df = datasource.query_data(settings.query, start_date, end_date)
    
    print(df)

    # if settings.debug:
    #     debugUI.show_dataframe_as_plot(df)
        # debugUI.show_dataframe_as_table(df)

    # df.ds = pd.to_datetime(df.ds)

    model = prophet.Prophet(
        daily_seasonality=False, \
        weekly_seasonality=False, \
        yearly_seasonality=False
    )
    model.add_seasonality(name='daily', period=1, fourier_order=5)

    model.fit(df)

    future_time = model.make_future_dataframe(
        periods=int(settings.forecast.forecast_period),
        freq=settings.forecast.forecast_step_literal, #"30MIN",
        include_history=True,
    )

    forecast = model.predict(future_time)

    storage.save_data(forecast)


    # forecast.to_pickle("./unittest/data/forecast.pkl")

    if settings.debug:
        debugUI.show_dataframe_as_plot(forecast)
        # debugUI.show_dataframe_as_table(forecast)



if __name__ == "__main__":
    train()