.PHONY: list
list:
	@LC_ALL=C $(MAKE) -pRrq -f $(lastword $(MAKEFILE_LIST)) : 2>/dev/null | awk -v RS= -F: '/^# File/,/^# Finished Make data base/ {if ($$1 !~ "^[#.]") {print $$1}}' | sort | egrep -v -e '^[^[:alnum:]]' -e '^$@$$'

##############################################################################

VENV := .venv
INSTALL_STAMP := $(VENV)/.install.stamp
PYTHON := $(VENV)/bin/python

##############################################################################

$(PYTHON):
	python3 -m virtualenv $(VENV)

$(INSTALL_STAMP): $(PYTHON) requirements.txt
	$(PYTHON) -m pip install -r requirements.txt
	touch $(INSTALL_STAMP)

##############################################################################

run: $(INSTALL_STAMP)
	$(PYTHON) src/main.py

test-only: $(INSTALL_STAMP)
	$(PYTHON) -m coverage run \
		--omit *_unittest.py,*__init__.py \
		--source=src \
		-m unittest discover --verbose --start-directory ./src/  --pattern '*_unittest.py'
	$(PYTHON) -m coverage html

coverage:
	firefox htmlcov/index.html

test: test-only coverage

##############################################################################

clean-python:
	find src/ -name '*.pyc' -exec rm --force {} +
	find src/ -name '*.pyo' -exec rm --force {} +
	find src/ -name '__pycache__' -exec rm -r --force {} +

clean-coverage:
	rm -f .coverage
	rm -rf htmlcov

clean-venv:
	rm -r $(VENV)

clean : clean-python clean-coverage

clean-all: clean-python clean-coverage clean-venv

