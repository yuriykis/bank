docker rm $(docker ps -aq)
docker rmi banking_project_api
docker rmi banking_project_web