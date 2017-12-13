build:
	@docker container run \
		--rm \
		--interactive \
		--tty \
		-e UID=$(shell id --user $$USER) \
		-e GID=$(shell id --group $$USER) \
		-v $$PWD:$$PWD \
		--workdir $$PWD \
		microsoft/dotnet:sdk sh -c '\
			dotnet publish --configuration=Release; \
			chown -R $$UID:$$GID PainlessGantt/bin; \
			chown -R $$UID:$$GID PainlessGantt/obj; \
			chown -R $$UID:$$GID PainlessGanttCli/bin; \
			chown -R $$UID:$$GID PainlessGanttCli/obj;'

run:
	@[ -f PainlessGanttCli/bin/Release/netcoreapp2.0/publish/PainlessGanttCli.dll ] || make --no-print-directory build
	@docker container run \
		--rm \
		--interactive \
		--tty \
		-e UID=$(shell id --user $$USER) \
		-e GID=$(shell id --group $$USER) \
		-v $$PWD/PainlessGanttCli/bin/Release/netcoreapp2.0/publish:/publish:ro \
		-v $$PWD/assets:/assets \
		--workdir $$PWD \
		microsoft/dotnet:runtime sh -c '\
			dotnet /publish/PainlessGanttCli.dll \
				--project /assets/project.yml \
				--template /assets/template.xlsx \
				--output /assets/ganttchart.xlsx; \
			chown $$UID:$$GID /assets/ganttchart.xlsx;'

format:
	@find . -type d \( \
			-name .git -o \
			-name .vs -o \
			-name bin -o \
			-name obj \
		\) -prune -o -type f ! \( \
			-name '*.xlsx' \
		\) -print0 | xargs -0 dos2unix

.PHONY: build run format
