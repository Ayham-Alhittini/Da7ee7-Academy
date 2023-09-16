import { Section } from "./section";

export interface Course {
    id: number;
    subject: string;
    teacherId: string;
    major: string;
    coursePhotoUrl: string;
    sections: Section[];
}