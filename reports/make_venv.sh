#!/bin/bash -f

REPORTS_ROOT=$( cd "$( dirname "$0" )" && pwd )
cd "$REPORTS_ROOT"

# Deactivate and remove the old virtual environment, if present
echo "Removing existing Virtual Environment, if present ..."
deactivate 2> /dev/null || true
rm -fr venv

# Create a new environment and activate it
echo "Creating new Virtual Environment ..."
python -m venv venv
. venv/bin/activate

# Make sure packaging tools are up to date
pip install --upgrade pip setuptools

# Install the reporting suite and its direct dependencies
pip install --no-build-isolation -e .
