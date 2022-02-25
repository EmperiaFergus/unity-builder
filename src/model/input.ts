import Platform from './platform';

const core = require('@actions/core');

/**
 * Input variables specified in workflows using "with" prop.
 *
 * Note that input is always passed as a string, even booleans.
 */
class Input {
  static get unityVersion() {
    return core.getInput('unityVersion') || 'auto';
  }
  static get scriptingDefineSymbol(){
    return core.getInput('scriptingDefineSymbol')
  }

  static get customImage() {
    return core.getInput('customImage');
  }

  static get targetPlatform() {
    return core.getInput('targetPlatform') || Platform.default;
  }

  static get projectPath() {
    const rawProjectPath = core.getInput('projectPath') || '.';
    return rawProjectPath.replace(/\/$/, '');
  }

  static get buildName() {
    return core.getInput('buildName') || this.targetPlatform;
  }

  static get buildsPath() {
    return core.getInput('buildsPath') || 'build';
  }

  static get buildMethod() {
    return core.getInput('buildMethod'); // processed in docker file
  }

  static get versioningStrategy() {
    return core.getInput('versioning') || 'Semantic';
  }

  static get specifiedVersion() {
    return core.getInput('version') || '';
  }

  static get androidVersionCode() {
    return core.getInput('androidVersionCode');
  }

  static get androidAppBundle() {
    const input = core.getInput('androidAppBundle') || false;

    return input === 'true';
  }

  static get androidKeystoreName() {
    return core.getInput('androidKeystoreName') || '';
  }

  static get androidKeystoreBase64() {
    return core.getInput('androidKeystoreBase64') || '';
  }

  static get androidKeystorePass() {
    return core.getInput('androidKeystorePass') || '';
  }

  static get androidKeyaliasName() {
    return core.getInput('androidKeyaliasName') || '';
  }

  static get androidKeyaliasPass() {
    return core.getInput('androidKeyaliasPass') || '';
  }

  static get allowDirtyBuild() {
    const input = core.getInput('allowDirtyBuild') || false;

    return input === 'true';
  }

  static get customParameters() {
    return core.getInput('customParameters') || '';
  }

  static get sshAgent() {
    return core.getInput('sshAgent') || '';
  }

  static get chownFilesTo() {
    return core.getInput('chownFilesTo') || '';
  }

  static get remoteBuildCluster() {
    return core.getInput('remoteBuildCluster') || '';
  }

  static get awsStackName() {
    return core.getInput('awsStackName') || '';
  }

  static get kubeConfig() {
    return core.getInput('kubeConfig') || '';
  }

  static get githubToken() {
    return core.getInput('githubToken') || '';
  }

  static get remoteBuildMemory() {
    return core.getInput('remoteBuildMemory') || '800M';
  }

  static get remoteBuildCpu() {
    return core.getInput('remoteBuildCpu') || '0.25';
  }

  static get kubeVolumeSize() {
    return core.getInput('kubeVolumeSize') || '5Gi';
  }

  static get kubeVolume() {
    return core.getInput('kubeVolume') || '';
  }

  static get s3InstallerAccessKey() {
    return core.getInput('s3InstallerAccessKey') || '';
  }

  static get s3InstallerSecretKey() {
    return core.getInput('s3InstallerSecretKey') || '';
  }

  static get s3InstallerRegion() {
    return core.getInput('s3InstallerRegion') || '';
  }

  static get s3InstallerBucketName() {
    return core.getInput('s3InstallerBucketName') || '';
  }

  static get serverDevelopmentAddress() {
    return core.getInput('serverDevelopmentAddress') || '';
  }

  static get serverProductionAddress() {
    return core.getInput('serverProductionAddress') || '';
  }

  static get serverTimeout() {
    return core.getInput('serverTimeout') || '';
  }

  static get patchkitSecret() {
    return core.getInput('patchkitSecret') || '';
  }

  static get patchkitAPIKey() {
    return core.getInput('patchkitAPIKey') || '';
  }

  static get APIstagingServerUrl() {
    return core.getInput('APIstagingServerUrl') || '';
  }

  static get APIStagingAuthServerUrl() {
    return core.getInput('APIStagingAuthServerUrl') || '';
  }
  static get APIProductionServerUrl() {
    return core.getInput('APIProductionServerUrl') || '';
  }
  static get APIStagingAuthServerUrl() {
    return core.getInput('APIStagingAuthServerUrl') || '';
  }
  static get APIClientID() {
    return core.getInput('APIClientID') || '';
  }
  static get APIClientSecret() {
    return core.getInput('APIClientSecret') || '';
  }
}

export default Input;
