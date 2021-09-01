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

## Make Commands:

- `make` or `make list` to show the list of make commands;
- `make run` to run the file `main.py`;
- `make test-only` to run only the tests without coverage;
- `IS_PROMETHEUS_AVAILABLE=true make test-only` to run only the tests without coverage with prometheus available;
- `make coverage` to open the coverage with firefox;
- `make test` to run test and show coverage;
- `IS_PROMETHEUS_AVAILABLE=true make test` to run test and show coverage with prometheus available;
- `make clean-python` to clean python files;
- `make clean-coverage` to clean coverage files;
- `make clean-venv` to clean the venv environment;
- `make clean` to clean python and coverage;
- `make clean-all` to clean everythin;

- `make run` to run the file `main.py`;IS_PROMETHEUS_AVAILABLE=true
