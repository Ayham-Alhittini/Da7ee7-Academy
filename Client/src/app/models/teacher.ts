import { Member } from "./member";

export interface Teacher {
    id: string;
    user: Member;
    major: string;
    gender: string;
}