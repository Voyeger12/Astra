# Repository Skills

Diese Skills sind verbindliche Arbeitsanweisungen für KI-gestützte Entwicklung in diesem Repository. Sie ersetzen keine Architekturentscheidungen, sondern übersetzen sie in wiederverwendbare Arbeitsabläufe.

## Verfügbare Skills

- `astra-architecture`: Schichtgrenzen, Abhängigkeiten und Architekturprüfungen
- `astra-agent-development`: Agent Runtime, Tools, Sessions, Provider und MCP
- `astra-quality-gates`: Tests, Evals, Lifecycle, Security und Definition of Done

## Verwendung

Vor Änderungen muss der passendste Skill gelesen werden. Bei übergreifenden Änderungen werden mehrere Skills kombiniert.

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
