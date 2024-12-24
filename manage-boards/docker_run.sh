#!/bin/zsh

# Load environment variables from .env 
source .env

# Run the Docker container with specified environment variables and port mapping
docker run -d \
  --name manage-boards \
  -p 5032:80 \
  -e UserPoolId=$USER_POOL_ID \
  -e Region=$REGION \
  -e ManageAuthLocalConnection=$MANAGE_AUTH_LOCAL_CONX \
  -e ManageColumnsLocalConnection=$MANAGE_COLUMNS_LOCAL_CONX \
  -e ProjectBLocalConnection=$PROJECT_B_LOCAL_CONX \
  tylersimeone/projectb/manage-boards:latest

if [ $? -ne 0 ]; then
  echo "Docker run command failed!"
  exit 1
fi

echo "Docker container started successfully."