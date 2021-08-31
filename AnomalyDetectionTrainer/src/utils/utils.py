
import os 

def raise_if_none(variable, variable_name=""):
    if variable is None:
        raise Exception(f"Variable: {variable_name} cannot be None")

def getenv_or_raise(key):
    env = os.getenv(key)
    raise_if_none(env)
    return env