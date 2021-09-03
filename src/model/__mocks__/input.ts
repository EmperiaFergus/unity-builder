// Import this named export into your test file:
import Platform from '../platform';

export const mockGetFromUser = jest.fn().mockResolvedValue({
  version: '',
  targetPlatform: Platform.types.Test,
  projectPath: '.',
  buildName: Platform.types.Test,
  buildsPath: 'build',
  buildMethod: undefined,
  buildVersion: '1.3.37',
  customParameters: '',
  s3InstallerAccessKey:'',
  s3InstallerSecretKey:'',
  s3InstallerRegion:'',
  s3InstallerBucketName:'',
  serverTimeout:'',
  serverDevelopmentAddress:'',
  serverProductionAddress:'',
  patchkitSecret:'',
  patchkitAPIKey:'',
  sshAgent: '',
  chownFilesTo: '',
});

export default {
  getFromUser: mockGetFromUser,
};
