
import os 

def raise_if_none(variable, variable_name=""):
    if variable is None:
        raise Exception(f"Variable: {variable_name} cannot be None")


def getenv_or_raise(key, output_type=None):
    env = os.getenv(key)
    raise_if_none(env)
    if output_type is None or output_type == str:
        return env
    elif output_type == bool:
        return parse_bool_or_raise(env)
    else:
        raise NotImplementedError

        
def parse_bool_or_raise(value):
    if isinstance(value, bool):
        return value
    if isinstance(value, int):
        return value > 0
    if isinstance(value, str):
        return value.lower() in ['true', '1', 't', 'y', 'yes', 'yeah', 'yup']
    
    raise Exception(f"Cannot parse: {value} ({type(value)}) to boolean")