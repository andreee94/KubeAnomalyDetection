# from models.datasources import Datasource
import time
import requests
import datetime
import numpy as np
import pandas as pd

from models.datasources.datasource import Datasource



class PrometheusDatasource(Datasource):

    PROMETHUES_TYPE = "PROMETHEUS"

    def __init__(self, url: str, is_authenticated: bool = False, username: str = None, password: str = None):
        super().__init__(url, PrometheusDatasource.PROMETHUES_TYPE,
                         is_authenticated, username, password)

    def query_data(self, query: str, start_date: datetime, end_date: datetime):
        
        step = 60

        response = requests.get(f"{self.url}/api/v1/query_range",
                                params={
                                    'query': query,
                                    'start': time.mktime(start_date.timetuple()),
                                    'end': time.mktime(end_date.timetuple()),
                                    'step': step
                                    })

        results = response.json()['data']['result']

        df = pd.DataFrame(results[0]['values'], columns=["ds", "y"])
        df.y = df.y.astype(float)
        # df.ds = df.ds * 1e9
        df.ds = pd.to_datetime(df.ds * 1e9)

        return df
