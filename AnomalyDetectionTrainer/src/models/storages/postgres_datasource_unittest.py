import unittest
import pandas as pd
from sqlalchemy.sql.expression import true
from models.storages.postgres_storage import PostgresStorage
from sqlalchemy import create_engine
import unittest_extension.skip_decorators


class TestPostgresStorage(unittest.TestCase):

    @staticmethod
    def read_data(storage, query):
        engine = create_engine(storage.connection_string)
        return pd.read_sql(query, con=engine)

    @staticmethod
    def truncate_table(storage):
        engine = create_engine(storage.connection_string)
        engine.connect().execute(f"TRUNCATE public.{PostgresStorage.FORECAST_TABLE_NAME}")

#########################################################################        

    @unittest_extension.skip_decorators.skipIfEnvIsUnset("IS_POSTGRES_AVAILABLE", "postgres is not available")
    def test_save_data_with_valid_data_should_succeed(self):
        storage = PostgresStorage(
            host="localhost:5432",
            username="postgres",
            password="password",
            container="postgres"
        )
        # extension methods for testing only 

        df = pd.read_pickle('unittest/data/forecast.pkl')

        TestPostgresStorage.truncate_table(storage)
        storage.save_data(df)

        query = F"""
        SELECT * FROM {PostgresStorage.FORECAST_TABLE_NAME}
        WHERE ds <= '{df.ds.max()}'
          AND ds >= '{df.ds.min()}'
        """

        inserted_df = TestPostgresStorage.read_data(storage, query)
        inserted_df.drop(columns=['index'], inplace=True) # the query also return the index, so we drop it to compare to the original

        self.assertEqual(df.shape[0], inserted_df.shape[0])
        pd.util.testing.assert_frame_equal(df, inserted_df)

#########################################################################

    def test_connection_string_should_be_correct(self):
        storage = PostgresStorage(
            host="localhost:5432",
            username="username",
            password="password",
            container="db"
        )
        expected_connection_string = "postgresql://username:password@localhost:5432/db"

        self.assertEqual(storage.connection_string, expected_connection_string)
        self.assertEqual(storage.storage_type, PostgresStorage.TYPE)



if __name__ == '__main__':
    unittest.main()
