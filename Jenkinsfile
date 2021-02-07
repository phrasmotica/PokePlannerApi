// JENKINS PARAMETERS
// - BRANCH_NAME: the branch to build

node {
    dir("PokeApiNet") {
        stage("Checkout PokeApiNet") {
            git(
                url: "https://github.com/phrasmotica/PokeApiNet",
                branch: "poke-planner-web"
            )
        }

        stage("Build PokeApiNet") {
            powershell "dotnet build"
        }
    }

    dir("PokePlannerApi") {
        stage("Checkout PokePlannerApi") {
            checkout scm
        }

        stage("Build PokePlannerApi") {
            powershell "dotnet build"
        }
    }

    stage("Clean workspace") {
        deleteDir()
    }
}