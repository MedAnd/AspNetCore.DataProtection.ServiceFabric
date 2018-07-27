#!/bin/bash

sfctl application upload --path ServiceFabric.DataProtection --show-progress
sfctl application provision --application-type-build-path ServiceFabric.DataProtection
sfctl application create --app-name fabric:/ServiceFabric.DataProtection --app-type ServiceFabric.DataProtectionType --app-version 1.0.4
