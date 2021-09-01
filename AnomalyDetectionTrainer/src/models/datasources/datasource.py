from abc import ABC, abstractmethod
from dataclasses import dataclass
import datetime
import numpy

class Datasource(ABC):

    def __init__(self, url: str, datasource_type: str, is_authenticated: bool = False, username: str = None, password: str = None):
        self.url = url
        self.datasource_type = datasource_type
        self.is_authenticated = is_authenticated
        self.username = username
        self.password = password

    @abstractmethod
    def query_data(self, query: str, start_date: datetime, end_date: datetime):
        """ this abstract method should return the data with a query from the datasource. """

    