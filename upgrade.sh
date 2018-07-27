#!/bin/bash
cd `dirname $0`
sfctl application upload --path Demo --show-progress
sfctl application provision --application-type-build-path Demo
sfctl application upgrade --app-id fabric:/Demo --app-version $1 --parameters "{}" --mode Monitored
cd -