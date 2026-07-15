# Repository Skills

Diese Skills sind verbindliche Arbeitsanweisungen für KI-gestützte Entwicklung in diesem Repository. Sie ersetzen keine Architekturentscheidungen, sondern übersetzen sie in wiederverwendbare Arbeitsabläufe.

## Verfügbare Skills

- `astra-architecture`: Schichtgrenzen, Abhängigkeiten und Architekturprüfungen
- `astra-agent-development`: Agent Runtime, Tools, Sessions, Provider und MCP
- `astra-test-first`: Test-first, Contract-first, Regressionstests und Eval-Abgrenzung
- `astra-quality-gates`: Tests, Evals, Lifecycle, Security und Definition of Done

## Verwendung

Vor Änderungen muss der passendste Skill gelesen werden. Bei übergreifenden Änderungen werden mehrere Skills kombiniert.

Für neue Features, Bugfixes, Agentenpfade, Tools, Persistenz und sicherheitsrelevante Änderungen ist `astra-test-first` zusätzlich verpflichtend.

Neue Skills müssen:

- einen klaren Trigger besitzen
- den betroffenen Scope benennen
- auf relevante ADRs verweisen
- konkrete Prüfschritte und Abbruchkriterien enthalten
- keine zweite, widersprüchliche Architektur definieren

## Verzeichnisstandard

```text
.github/skills/<skill-name>/SKILL.md
```

Skill-Namen verwenden Kleinbuchstaben und Bindestriche.