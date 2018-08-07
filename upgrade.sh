#!/bin/bash
cd `dirname $0`
sfctl application upload --path ServiceFabric.DataProtection --show-progress
sfctl application provision --application-type-build-path ServiceFabric.DataProtection
sfctl application upgrade --app-id fabric:/ServiceFabric.DataProtection --app-version $1 --parameters "{}" --mode Monitored
cd -