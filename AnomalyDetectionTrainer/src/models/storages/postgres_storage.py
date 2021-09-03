from dataclasses import dataclass
from overrides import overrides
from pandas.io.formats.format import EngFormatter
from models.storages.storage import Storage
from sqlalchemy import create_engine
import pandas as pd


class PostgresStorage(Storage):

    TYPE = "POSTGRES"
    FORECAST_TABLE_NAME = "tb_forecast_data"

    def __init__(self, host: str, username: str, password: str, container: str):
        super().__init__(host, PostgresStorage.TYPE,
            username=username, password=password, container=container)

    @property
    def connection_string(self):
        return f"postgresql://{self.username}:{self.password}@{self.host}/{self.container}"


    @overrides
    def save_data(self, df):
        engine = create_engine(self.connection_string)
        df.to_sql(PostgresStorage.FORECAST_TABLE_NAME, engine, if_exists='append')




    # def query_data(self, query: str, start_date: datetime, end_date: datetime):

    #     step = 600

    #     response = self.get_prometheus_request(query, start_date, end_date, step)

    #     results = response['data']['result']

    #     df = pd.DataFrame(results[0]['values'], columns=["ds", "y"])
    #     df.y = df.y.astype(float)
    #     # df.ds = df.ds * 1e9
    #     df.ds = pd.to_datetime(df.ds * 1e9)

    #     return df
