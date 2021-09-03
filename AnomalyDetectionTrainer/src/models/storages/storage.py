from abc import ABC, abstractmethod

class Storage(ABC):

    def __init__(self, host: str, storage_type: str, username: str, password: str, container: str): # container for a database is the database name, for s3 is the bucket name 
        self.host = host
        self.storage_type = storage_type
        self.username = username
        self.password = password
        self.container = container

    @abstractmethod
    def save_data(self, df):
        """ this abstract method should save the data withing the datasource. """

    