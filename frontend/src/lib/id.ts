export function GenerateIdCrypto() {
  return crypto.randomUUID();
}
export function GenerateIdNumber(): number {
  return Math.floor(Math.random() * Number.MAX_SAFE_INTEGER);
}
