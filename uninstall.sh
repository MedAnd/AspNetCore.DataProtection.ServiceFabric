#!/bin/bash


sfctl application delete --application-id Demo
sfctl application unprovision --application-type-name DemoType --application-type-version 1.0.0
sfctl store delete --content-path Demo