# Okey

[![état du pipeline](https://git.unistra.fr/okaybytes/okey/badges/main/pipeline.svg)](https://git.unistra.fr/okaybytes/okey/-/commits/main)
[![rapport de couverture](https://git.unistra.fr/okaybytes/okey/badges/main/coverage.svg)](https://git.unistra.fr/okaybytes/okey/coverage)
[![dernière version](https://git.unistra.fr/okaybytes/okey/-/badges/release.svg)](https://git.unistra.fr/okaybytes/okey/-/releases/permalink/latest)
[![pre-commit](https://img.shields.io/badge/pre--commit-enabled-brightgreen?logo=pre-commit)](https://github.com/pre-commit/pre-commit)

Vous trouverez ici le projet [Okey](https://git.unistra.fr/okaybytes/okey) qui est un jeu de table entre 2 à 4 personnes surtout connu et réparti en Turquie.

Voir la [documentation](https://okaybytes.pages.unistra.fr/okey) complète pour plus de détails sur l'utilisation et l'installation.

**Table des matières :**

[[_TOC_]]

## Documents internes

### Général

* [Cahier des charges](https://docs.google.com/document/d/1Rsr_0wPjUDgFJGwcGmsh3udF6GZKAEvckagoF2dQsZc/edit?usp=sharing)
* [Team log](https://docs.google.com/spreadsheets/d/1c_RrIB0kDTPiEfFVfjjMwCOlqc8TLVP4LDvLlRVz5Ng/edit?usp=sharing) (ancien)
* [Team log](https://docs.google.com/spreadsheets/d/1Lx6XSw-6TPpPX6M-f2ByZmqWlIIDPr0a4ye2qn6C0Tg/edit?usp=sharing)

### Backend

* [Tâches backend](https://docs.google.com/document/d/1tw6rdk8RJixN4Gf4Xx8rJSAIJKp01h6838isS8QlBjY/edit?usp=sharing)
* [Spécifications](https://docs.google.com/document/d/138RZhz9YMOilw_RB4Bq7qBsjzFfuHsJlgB5n1W3kzYc/edit?usp=sharing)

### Frontend

* [Tâches frontend](https://docs.google.com/document/d/1juRMyaspqpbGG-6astmrf9sYpfhF4I-d_2sC1IXX4jM/edit?usp=sharing)
* [UML](https://drive.google.com/file/d/1YIBEHZWpCWMH5IgFtIi-53_cFjVEIpYE/view?usp=sharing_eip_m&ts=65d4c787)
* [Diagramme d'activités](https://drive.google.com/file/d/1TDauPSD0wWvksGSJn-EX9Y7hbyhopGBx/view?ts=65de1e74)

#### IHM

* [Figma](https://www.figma.com/file/ZTVmviAU1qUqfNIfh04j1V/Untitled?type=design&node-id=0%3A1&mode=design&t=2wq9oDEamRp9MzS5-1)
* [Wireframes](https://drive.google.com/file/d/1QMwPXv768F97Ix5pabN3Do9hoXsxo5LF/view?usp=sharing)
* [Tests utilisateur](https://docs.google.com/document/d/1pJDbbH8KoasmX3JDQduUXqLfxRmnVAhrXfWyx_RvB9Q/edit?usp=sharing)

### Commun entre backend et frontend

* [Spécification des communications](https://docs.google.com/document/d/1m7YraiAPQxlpeK8IJXncBgzjGUizkMRcMqzwSLpFpZ8/edit?usp=sharing)

## Environnement de développement

*Pour une utilisation en tant que développeur, un environnement dev*.

### Dépendances

Assurez vous d'être réglé avec les bonnes versions des dépendances de premier niveau dans la liste en dessous qui sont donc Git, Python, .NET Core et Unity. Les autres points (dépendances de deuxième niveau et encore) seront abordés plus en détails avec des instructions plus loin.

Cette liste indique les versions minimales recommandées ou des versions exactes (sur certains) des outils nécessaires pour travailler sur ce projet. Si vous ne respectez pas ces versions votre environnement de développement risque de ne pas fonctionner correctement. Mais surtout, vous serez en décalage avec le reste de l'équipe et vous risquez de rencontrer des problèmes lors de votre implémentation (vieille version .NET ou mauvaise version Unity par exemple) qui ne sera plus compatible avec le projet.

* [Git](https://git-scm.com/) récent
* [Python](https://www.python.org/) >= 3.9 stable (3.10.13 recommandé)
  * [pre-commit](https://pre-commit.com/) >= 3.6.1
  * [python-gitlab](https://python-gitlab.readthedocs.io/en/v4.4.0/) >= 4.4.0
  * [python-dotenv](https://pypi.org/project/python-dotenv/) >= 1.0.1
* [.NET Core](https://dotnet.microsoft.com/en-us/download) 8.0.1 LTS
  * [csharpier](https://csharpier.com/) >= 0.27.2
  * [docfx](https://dotnet.github.io/docfx/index.html) >= 2.75.2
  * [xunit](
  * [roslynator](https://josefpihrt.github.io/docs/roslynator/) >= 0.8.3
* [Unity](https://unity.com/fr) 2022.3.19f1 LTS

### Étapes d'installation

#### Dépôt Git local

Assurez vous d'avoir une version récente de Git.

Veillez à utiliser vos identifiants GitLab Unistra dans votre dépôt local pour que les commits soient bien attribués à votre compte.

```sh
git clone git@git.unistra.fr:okaybytes/okey.git
cd okey  # À partir de maintenant toutes les commandes depuis le répertoire racine du projet
git config --local user.name "NOM PRENOM"  # Utilisez votre nom sur GitLab Unistra
git config --local user.email "ernest@etu.unistra.fr"  # Utilisez votre email sur GitLab Unistra
```

Configurez votre éditeur de texte par défaut pour les messages de commit **si ce n'est pas déjà fait** :

```sh
git config --global core.editor ["vi", "vim", "nvim", "nano", "code --wait", "subl --wait"]  # Choisir votre éditeur préféré ou un de cette liste
```

#### Dotnet

Assurez vous d'avoir la version indiquée de Dotnet dans la liste au dessus.

Dotnet est utilisé pour le backend du projet, mais pas que. Des outils écrit en Dotnet sont aussi utilisés pour le formatage des fichiers C# ou la génération de la documentation. On installe ces outils ici.

```sh
dotnet tool restore  # Installez les outils dotnet
```

#### Python

Python est utilisé pour les hooks git et une variété d'autres scripts. Ceci est plutôt pour l'environnement de développement local que pour le projet en lui-même.

Cette partie des étapes comporte une partie optionnel qui peut vous être utile si vous voulez travailler dans un environnement virtuel Python. Sinon, vous pouvez sauter cette étape.

Assurez vous d'avoir la version indiquée de Python dans la liste au dessus. Si vous n'avez pas la bonne version de Python et que vous voulez gerer de multiples versions Python ou si installer la bonne version de Python s'avère difficile, la méthode `pyenv` dans l'étape optionnel en dessous vous est recommandé.

##### Virtualenv (optionnel)

Si vous voulez créer et travailler dans un environnement virtuel pour le projet (conseillé), **installez et configurez** [`pyenv`](https://github.com/pyenv/pyenv?tab=readme-ov-file#installation) (gestionnaire de versions Python). **Attention, installer `pyenv` n'est pas juste une simple installation de paquet. Il faut configurer votre shell en suivant [ces instructions supplémentaires](https://github.com/pyenv/pyenv?tab=readme-ov-file#set-up-your-shell-environment-for-pyenv).** Et puis, exécutez les commandes suivantes avant la suite :

```sh
pyenv install 3.10.13  # Installez la version de Python recommandée en parallèle avec votre version actuelle
pyenv virtualenv 3.10.13 okey  # Créez un environnement virtuel pour le projet
pyenv local okey  # Spécifiez l'environnement virtuel à utiliser pour ce répertoire
python -m pip install --upgrade pip  # Mettre à jour pip
```

Ou bien vous pouvez utiliser `venv` ou `virtuelenv` pour créer et gérer l'environnement virtuel mais veillez à ne pas oublier de l'activer avant chaque fois que vous allez travailler sur le projet (dans ce cas l'activation et la désactivation n'est pas automatique).

##### Dépendances et hooks Git (obligatoire)

**!!Avant de passer à la suite**, téléchargez le fichier `.env` qui contient les variables d'environnement nécessaires pour le projet depuis le [**Seafile partagé**](https://seafile.unistra.fr/smart-link/2ab30c65-6f80-4575-a26b-90b170481569/) Unistra et mettez le dans le répertoire racine du projet. Assurez vous qu'il est nommé `.env` et qu'il est bien placé dans le répertoire racine du projet. Ne partagez pas ou ne diffusez pas ce fichier, il contient des informations sensibles comme des clés API. Ce fichier est utilisé dans un premier temps dans les hooks Git.

Ensuite, installez les dépendances Python (il se peut que `pip` ne soit pas correctement aliasé dans votre installation et que vous deviez le remplacer par `pip3`) :

```sh
pip install -r requirements.txt  # Installez les packages Python
```

Finalement, configurez les hooks Git (une fois Python est réglé) :

```sh
pre-commit install  # Installez les hooks git
pre-commit run -a  # Lancez les hooks git pour la toute première fois
```

*Lire aussi les sections en dessous sur les tests unitaires et la génération de doc pour avoir votre environnement de dev complètement réglé.* (EN CONSTRUCTION ...)

### Astuce hooks Git

Si à cause des git hooks, votre commit a été refusé après que vous avez renseigné votre message de commit, pas de panique c'est pas perdu, vous pouvez le récupérer et l'éditer pour le corriger en exécutant la commande suivante :

```sh
git commit -eF "$(git rev-parse --show-toplevel)/.git/COMMIT_EDITMSG"
```

Vous pouvez aussi désactiver les hooks Git pour un commit en exécutant la commande suivante :

```sh
git commit --no-verify
```

Notez que les hooks Git sont là pour vous aider à écrire des commits propres et bien formatés. Essayez de n'utiliser cette astuce qu'en cas de force majeure ou si vous venez d'exécuter les git hooks et tout s'est bien passé, et vous savez que rien n'a changé entre temps, pour ne pas relancer les hooks et perdre du temps quand vous savez déjà le résultat, vous pouvez faire cela. Mais puisqu'un certain nombre de ces hooks sont aussi lancé coté serveur, si vous les désactivez vous n'aurez pas la garantie que votre commit sera accepté par le serveur lors du CI et votre merge request peut être bloqué.

### Astuce Roslynator

Roslynator est un analyseur et correcteurs d'erreurs du code C#. Pour lancer l'analyse :

```sh
dotnet roslynator analyze <chemin vers le fichier .csproj ou .sln de votre projet>
```

Les git hooks lancent automatiquement aussi les vérifications de Roslynator avant un commit ou push. Mais ce ne sont que des vérifications et non pas des corrections automatiques. Pour corriger les erreurs signalées par Roslynator, vous pouvez exécuter la commande suivante :

```sh
dotnet roslynator fix <chemin vers le fichier .csproj ou .sln de votre projet>
```

Cette commande va essayer d'appliquer des corrections si possible sans plus de garantie.

Veillez à bien lire les messages d'erreurs et les suggestions de corrections de Roslynator pour bien comprendre ce qui ne va pas et comment le corriger. Parfois, les suggestions de corrections ne sont pas toujours les meilleures et peuvent introduire des erreurs ou des comportements indésirables dans le code. Il est donc important de bien comprendre les erreurs et les suggestions de corrections avant de les appliquer. Cet outil est important pour maintenir un code propre, lisible et de haute qualité avec les meilleures pratiques C#.

### Documentation

La documentation est générée par `docfx` et se trouve dans le répertoire `docs/`. Pour la générer, exécutez la commande suivante :

```sh
dotnet docfx docs/docfx.json
```

Pour la visualiser dans votre navigateur en local, exécutez la commande suivante et ouvrez la page `localhost:8080` dans votre navigateur :

```sh
dotnet docfx docs/docfx.json --serve
```

Voir <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags> pour plus de détails sur la documentation XML C# dans le code qui ressemble un peu à `Doxygen` ou les `Docstring` Python mais un format different.

## Dépôt Git

Le dépôt git de [ce projet](https://git.unistra.fr/okaybytes/okey) suit le modèle de branchement git "[Github Flow](https://docs.github.com/en/get-started/using-github/github-flow)" avec une obligation de merge request pour chaque modification et une issue GitLab pour chaque tâche ou problème. La branche `main` est protégé et ne peut être modifié que par des merge request validées par le CI. Les merge request doivent être validées par au moins un autre développeur avant d'être fusionnées.

Les noms des branches doivent avoir la référence vers l'issue correspondante avec le format `<numéro de issue>-<titre-de-la-branche>` (par exemple `42-add-a-new-feature`). Chaque commit doit être lié à une issue avec le format `GL-<numéro de issue>: <reste du titre>` au tout début du message de commit (par exemple `GL-881: Fix a typo in the README`).

Pour suivre ce modèle, suivez les étapes suivantes :

* Créez une issue sur GitLab pour votre tâche ou votre problème en utilisant bien les bons tags et le bon milestone. Assurez vous de ne pas créer une issue dupliqué en vérifiant si une telle issue n'existe pas encore.
  * Mettez le label `Doing` tant que vous travaillez sur la tâche mentionnée dans l'issue. Enlevez le label quand vous avez fini.
* Créez une branche et une merge request à partir de l'issue sur GitLab, puis récupérez cette branche avec `git fetch` et `git merge` ou `git pull`. Ou bien créez votre branche en locale en veillant au bon format de nom de branche et poussez la sur le dépôt distant et créez une merge request à partir de la branche sur GitLab. La merge request est l'endroit où la discussion sur le code se passe, donc il est mieux de la créer dès le départ pour faciliter la collaboration même si votre solution n'est pas encore prête ou vous n'avez rien à montrer encore.
  * Veillez à bien lier la merge request à l'issue correspondante.
* Une fois votre merge request est prête, demandez à un autre développeur de la relire et de la valider. Si tout est bon, elle sera fusionnée avec la branche `main` et la branche de travail sera supprimée.
  * Mettez à jour le fichier `CHANGELOG.md` avec les changements apportés au fur et à mesure.
* Quand on arrive à une version stable (les jalons), on crée une release avec un tag correspondant et on met à jour le fichier `CHANGELOG.md` avec les changements apportés. L'intégration continue (CI) se chargera de publier la nouvelle version sur le serveur de production.

Tout le développement se passe sur des branches tiers. Une fois une tâche ou fonctionnalité est complété, elle est testé, et sa merge request est fusionnée avec la branche `main`.

Pensez à régulièrement mettre à jour votre branche de travail avec la branche `main` (en faisant des `git pull` sur `main` et `git merge main` sur votre branche de travail) pour éviter les conflits lors de la fusion de la merge request ainsi que pour avoir accès aux derniers changements (bug fixes, mise à jour de la configuration du dépôt...).
