# GitHub Copilot Instructions for Drug Response (Continued) RimWorld Mod

## Mod Overview and Purpose

**Drug Response (Continued)** is an updated version of the original "Drug Response" mod by pelador. It is a Quality of Life (QoL) tool that allows players to configure AI responses for their colonists to handle pain and various maladies or conditions. The mod enhances the existing vanilla drug policy process by providing additional options that complement it without replacing it.

## Key Features and Systems

- **Drug Policy Customization**: Allows players to configure how their colonists respond to different maladies or conditions using various drugs.
- **Caravan Support**: Ensures that colonists can respond to maladies while traveling in caravans by checking for necessary drugs.
- **Multiplayer Compatibility**: Supports multiplayer games seamlessly.
- **Integration with Other Mods**: Works well with popular drug mods like Medicines+, Remedies, and Medicine Expanded.
- **Language Support**: Includes German translation.
- **Optional Features**: Provides additional maladies and responses that can be toggled based on the presence of relevant drug mods.

## Coding Patterns and Conventions

### C# Coding Conventions

- **Class and Method Naming**: Use clear and descriptive names. For example, `CheckDrugDefinitions` and `MSCaravanUtility`.
- **Method Documentation**: Provide XML documentation for methods to explain their purpose, parameters, and return values.
- **Code Organization**: Group related code into classes and methods. For example, `MSDrugUtility` contains methods related to drug validation and finding drugs.

### XML Coding Conventions

- **Patch Definitions**: Define patches in XML files with clear and descriptive names, such as `Patches_DR_DrugsPlus.xml`.
- **Harmony Patching**: Use Harmony for patching RimWorld code. Ensure that patches are named and organized logically to avoid conflicts.

## XML Integration

The mod uses XML files to define patches and integrate with RimWorld. Key XML files include:

- **Patches_DR_AY.xml**: Patches for the Apothecary mod.
- **Patches_DR_DO.xml**: Patches for the Diseases Overhauled mod.
- **Patches_DR_DrugsPlus.xml**: Patches for the Medicines+ mod.
- **Patches_DR_MedicinePlus.xml**: Patches for the Medicine Expanded mod.
- **Patches_DR_MS.xml**: General patches for the mod.

Each XML file contains specific patches that modify RimWorld behavior to integrate with the mod's features.

## Harmony Patching

**Harmony** is used extensively in the mod to patch RimWorld code. Key points include:

- **Patch Targets**: Clearly define the methods being patched and the Harmony patch IDs.
- **Patch Logic**: Implement patch logic that enhances the mod's features without interfering with vanilla gameplay.
- **Testing**: Thoroughly test patches to ensure they work as expected and do not introduce bugs.

## Suggestions for Copilot

1. **Provide Detailed Descriptions**: When generating code or XML, ensure that descriptions are provided for classes, methods, and patches.
2. **Follow Coding Conventions**: Adhere to the established C# and XML coding conventions to maintain consistency and readability.
3. **Integrate with Existing Code**: Ensure that any new code integrates seamlessly with the existing mod codebase.
4. **Test Patches**: Always test patches to ensure they function correctly and do not introduce compatibility issues with other mods.

By following these guidelines, Copilot can effectively assist in developing and maintaining the Drug Response (Continued) mod, enhancing its features and ensuring compatibility with RimWorld and other mods.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

