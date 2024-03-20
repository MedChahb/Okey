REGISTRY=registry.app.unistra.fr/okaybytes/okey

IMG_NAME_CICD=projet-integrateur-cicd
IMG_NAME_API=okeyapi
IMG_NAME_SERVER=okeyserver

.PHONY: all clean

all: cicd api server

cicd: $(IMG_NAME_CICD)
$(IMG_NAME_CICD): Dockerfile ../requirements.txt ../.config/dotnet-tools.json
	docker build -t $(REGISTRY)/$@ -f $< ..
	touch $@

api: $(IMG_NAME_API)
$(IMG_NAME_API): OkeyApi.Dockerfile ../server/OkeyApi/
	docker build -t $(REGISTRY)/$@ -f $< ..
	touch $@

server: $(IMG_NAME_SERVER)
$(IMG_NAME_SERVER): OkeyServer.Dockerfile ../server/OkeyServer/
	docker build -t $(REGISTRY)/$@ -f $< ..
	touch $@

clean:
	rm -f $(IMG_NAME_CICD) $(IMG_NAME_API) $(IMG_NAME_SERVER)
	@echo "Makefile: artifacts cleaned."
	@echo "Makefile: this does not remove the docker images or containers. Use 'docker rmi' and/or 'docker container rm' for that."
