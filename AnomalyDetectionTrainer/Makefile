.PHONY: list
list:
	@LC_ALL=C $(MAKE) -pRrq -f $(lastword $(MAKEFILE_LIST)) : 2>/dev/null | awk -v RS= -F: '/^# File/,/^# Finished Make data base/ {if ($$1 !~ "^[#.]") {print $$1}}' | sort | egrep -v -e '^[^[:alnum:]]' -e '^$@$$'

##############################################################################

VENV := .venv
INSTALL_STAMP := $(VENV)/.install.stamp
INSTALL_STAMP_DEBUG := $(VENV)/.install.debug.stamp
PYTHON := $(VENV)/bin/python
COVERAGE_MARKDOWN_FILE := ./COVERAGE.md

##############################################################################

$(PYTHON):
	python3 -m virtualenv $(VENV)

$(INSTALL_STAMP): $(PYTHON) requirements.txt
	$(PYTHON) -m pip install -r requirements.txt
	touch $(INSTALL_STAMP)

$(INSTALL_STAMP_DEBUG): $(PYTHON) requirements.debug.txt
	$(PYTHON) -m pip install -r requirements.debug.txt
	touch $(INSTALL_STAMP_DEBUG)

##############################################################################

run: $(INSTALL_STAMP)
	$(PYTHON) src/main.py

debug: $(INSTALL_STAMP_DEBUG)
	export DEBUG=true && $(PYTHON) -m streamlit run src/main.py

test-only: $(INSTALL_STAMP)
	$(PYTHON) -m coverage run \
		--omit *_unittest.py,*__init__.py \
		--source=src \
		-m unittest discover --verbose --start-directory ./src/  --pattern '*_unittest.py'

coverage:
	$(PYTHON) -m coverage html
	firefox htmlcov/index.html


coverage-text:
	$(PYTHON) -m coverage report


coverage-json:
	$(PYTHON) -m coverage json

coverage-md:
	$(PYTHON) -m coverage report | tr -s ' ' | sed 's/ /|/g' | tr -s '-' | sed 's/^-$//-|-|-|-/' | xargs -L 1 printf "|%s|\n" > $(COVERAGE_MARKDOWN_FILE)


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

