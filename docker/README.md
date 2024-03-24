# Les images Docker

Vous pouvez utiliser le `Makefile` fourni pour construire les images Docker en local. Pensez à supprimer les images et les conteneurs manuellement une fois vous avez fini vu que le `Makefile` ne le fait pas.

Pensez à bien nommer les images Docker quand vous faites un build local comme le fait le `Makefile` si vous voulez les push sur le registre GitLab Unistra par après.

À savoir que le [build context](https://docs.docker.com/build/building/context/) est le répertoire racine du projet. Dans les Dockerfiles que vous allez écrire, utilisez donc des chemins relatifs à partir de la racine du projet.

##  Construction automatique des images Docker sur GitLab CI/CD

Les pipelines CI/CD GitLab sont configurés pour construire les images Docker et les pousser sur le registre GitLab Unistra lorsqu'il y a un changement dans les Dockerfiles ou bien sur leurs dépendances sur certaines branches git.

Les images des applications back-end sont construits automatiquement sur la branche `back-end` quand un changement dans les fichiers `Dockerfile` ou dans ses autres dépendances est détecté. Ces images sont taggés systématiquement `dev-<short git hash>` et le méta tag `dev` pointe vers la dernière de ces versions test/développement.

La version stable de ces images est construite quand un tag de release est détecté sur la branche `main`. Ces images stables sont taggés à leur tour par la version de la release et le méta tag `latest` pointe vers la dernière version stable.
