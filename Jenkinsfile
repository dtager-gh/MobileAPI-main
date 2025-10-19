pipeline {
    agent any

    environment {
        IMAGE_BASE = "mobileapi"
    }

    parameters {
        string(name: 'GIT_BRANCH', defaultValue: 'main', description: 'Branch to build')
        string(name: 'GIT_REPO', defaultValue: 'git@github.com:WVUP/MobileAPI.git', description: 'Git repo URL')
    }

    stages {
        stage('Determine Next Image Tag') {
            steps {
                script {
                    // Get the highest numeric tag, if any
                    def lastTag = sh(
                        script: 'docker image ls --format "{{.Tag}}" ${IMAGE_BASE} | grep -E "^[0-9]+$" | sort -n | tail -1',
                        returnStdout: true
                    ).trim()

                    if (lastTag == "") {
                        env.IMAGE_TAG = "${IMAGE_BASE}:1"
                    } else {
                        def next = lastTag.toInteger() + 1
                        env.IMAGE_TAG = "${IMAGE_BASE}:${next}"
                    }

                    echo "Using image tag: ${env.IMAGE_TAG}"
                }
            }
        }

        stage('Checkout Source') {
            steps {
                git branch: "${params.GIT_BRANCH}", 
                url: "${params.GIT_REPO}",
                credentialsId: '32ef670b-3704-40cb-974f-82b493d4dabe'
            }
        }

        stage('Build Image') {
            steps {
                sh "docker build -t ${env.IMAGE_TAG} -f ./Dockerfile ."
            }
        }
        
        stage('Run Unit Tests') {
            steps {
                sh """
				  DOCKER_BUILDKIT=1 docker build --no-cache -t ${IMAGE_BASE}.test.${env.IMAGE_TAG} -f ./Dockerfile.test .
				"""
            }
        }
        
        stage('Deploy Stack') {
            steps {
                sh 'docker compose down || echo . >/dev/null'
                sh "IMAGE_TAG=${env.IMAGE_TAG} docker compose up -d"
            }
        }

        //stage('Cleanup Old Images') {
        //    steps {
        //        // You can move the Cleanup Old Images into a different pipeline and trigger it from here instead
        //        build job: 'MobileAPICleanup', wait: false
        //    }
        //}
        stage('Cleanup Old Images') {
            steps {
                script {
                    // Keep last 3 numeric tags, delete the rest
                    sh '''
                        docker image ls --format "{{.Repository}}:{{.Tag}} {{.CreatedAt}}" | grep '^mobileapi*.[0-9]' \
                        | sort -k2 -r | tail -n +4 | awk '{print $1}' | xargs -r docker rmi
                    '''
                    sh '''
                       docker image ls --format "{{.Repository}}:{{.Tag}} {{.CreatedAt}}" | grep '^mobileapi.test' \
                        | awk '{print $1}' | xargs -r docker rmi
                    '''
                    sh 'docker image prune -f'
                }
            }
        }
    }

    post {
	    always {
			sh 'docker ps'
		}
        success {
            echo "✅ Build and deploy successful, new image tag is ${env.IMAGE_TAG}"
        }
        failure {
            echo "❌ Build or deploy failed"
        }
    }
}


