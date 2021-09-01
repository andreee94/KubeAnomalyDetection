import os
import unittest


def skipIfEnvIsSet(env_name, reason):
    """
    Skip a test if the environmental variable is set.
    """
    condition = os.getenv(env_name, None) is None

    return unittest.skipIf(condition, reason)


def skipIfEnvIsUnset(env_name, reason):
    """
    Skip a test if the environmental variable is set.
    """
    condition = os.getenv(env_name, None) is not None

    return unittest.skipIf(condition, reason)