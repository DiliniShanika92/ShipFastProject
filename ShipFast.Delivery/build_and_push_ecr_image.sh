#!/bin/bash
set -e

aws ecr get-login-password --region ap-southeast-1 --profile shipfast-ecr-agent | docker login --username AWS --password-stdin 026090537960.dkr.ecr.ap-southeast-1.amazonaws.com
docker build -f ./Dockerfile -t shipfast-delivery. 
docker tag shipfast-delivery:latest 026090537960.dkr.ecr.ap-southeast-1.amazonaws.com/shipfast-delivery:latest
docker push 026090537960.dkr.ecr.ap-southeast-1.amazonaws.com/shipfast-delivery:latest



