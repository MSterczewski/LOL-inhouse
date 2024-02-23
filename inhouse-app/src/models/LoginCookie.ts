export interface LoginData {
  nickname: string;
  password: string;
}
export interface LoginCookie {
  nickname: string;
  id: number;
  token: string;
  role: ERole;
  isAdmin: boolean;
  imageUrl: string;
}

export enum ERegisterErrorCode {
  DataInvalid,
  NicknameExists,
}

export enum ELoginErrorCode {
  DataInvalid,
  PlayerNotFound,
  PasswordInvalid,
}

export enum ERole {
  User,
  Admin,
}
