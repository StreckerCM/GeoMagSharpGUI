# Feature: Remove Bundled Coefficient Files
Issue: #49
Branch: feature/49-remove-bundled-coefficients

## Tasks
- [ ] Delete bundled COF files and MagneticModels.json from repo
- [ ] Remove Content entries from GeoMagGUI.csproj
- [ ] Remove embedded resource entries from Resources.resx
- [ ] Remove auto-generated properties from Resources.Designer.cs
- [ ] Add auto-discover logic to frmMain.cs
- [ ] Update CLAUDE.md data directories section
- [ ] Build and verify model loading works

## Completion Criteria
- [ ] All tasks checked
- [ ] Build succeeds
- [ ] App launches and auto-discovers NuGet-provided COF files
- [ ] 2 clean Ralph Loop cycles
