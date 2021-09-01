import unittest

class ExtendedTestCase(unittest.TestCase):

  def assertRaisesWithMessage(self, msg, func, *args, **kwargs):
    try:
      func(*args, **kwargs)
      self.assertFail()
    except Exception as inst:
      self.assertEqual(inst.message, msg)
