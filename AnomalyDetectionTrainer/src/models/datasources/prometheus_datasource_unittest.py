import datetime
import unittest
from unittest import result
import prophet
import matplotlib.pyplot as plt
import pandas as pd
import unittest_extension.skip_decorators

from models.datasources.prometheus_datasource import PrometheusDatasource


class TestPrometheusDatasource(unittest.TestCase):

    @unittest_extension.skip_decorators.skipIfEnvIsSet("IS_PROMETHEUS_AVAILABLE", "prometheus is not available")
    def test_query_data_with_valid_data_should_succeed(self):
        datasource = PrometheusDatasource(
            url="http://192.168.1.102:9091",
            is_authenticated=False
        )
        query = "up"
        start_date = datetime.datetime.utcnow() - datetime.timedelta(hours=1)
        end_date = datetime.datetime.utcnow()

        result = datasource.query_data(query, start_date, end_date)

        self.assertIn("ds", result.columns)
        self.assertIn("y", result.columns)
        self.assertEqual(result.shape[1], 2)
        self.assertGreater(result.shape[0], 0)
        self.assertLessEqual(result.shape[0], 60 + 1) # we are querying 1 hour so 60 + 1 samples at maximum
        self.assertEqual(result.ds.dtypes.name, 'datetime64[ns]')
        self.assertEqual(result.y.dtypes.name, 'float64')
        # TODO TEST THE STEP


    def test_1(self):
        pass
        # plt.figure()
        # ax = plt.gca()

        # datasource = PrometheusDatasource(
        #     url="http://192.168.1.102:9090",
        #     is_authenticated=False
        # )

        # # query = """rate(loki_distributor_lines_received_total[5m])""" #"up"
        # query = """sum(avg_over_time(kube_metrics_server_pods_cpu[120m])) / 1e6"""
        # start_date = datetime.datetime.utcnow() - datetime.timedelta(days=7)
        # end_date = datetime.datetime.utcnow()

        # df = datasource.query_data(query, start_date, end_date)

        # # df.ds = pd.to_datetime(df.ds)

        # model = prophet.Prophet(
        #     daily_seasonality=False, \
        #     weekly_seasonality=False, \
        #     yearly_seasonality=False
        # )
        # model.add_seasonality(name='daily', period=1, fourier_order=30)

        # model.fit(df)

        # future_time = model.make_future_dataframe(
        #     periods=int(48),
        #     freq="30MIN",
        #     include_history=True,
        # )

        # forecast = model.predict(future_time)

        # print(forecast.shape, forecast.columns)
        # print(df.shape, df.columns)
        # print(type(df.ds), type(df.y))

        # df.plot(x='ds', y='y', ax=ax)

        # forecast.plot(x='ds', y=['yhat', 'yhat_lower', 'yhat_upper'], ax=ax)

        # plt.show()


if __name__ == '__main__':
    unittest.main()
