## Testing Coverage

### To run test with coverage:

```bash
python -m coverage run --omit *_unittest.py,*__init__.py --source=. -m unittest discover -v -s ./src/  -p '*_unittest.py'
```

### To show coverage report:

```bash
python -m coverage report -m
```

or 

```bash
python -m coverage html
```