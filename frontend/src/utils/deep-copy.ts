export function DeepCopy(value: any) {
  if (value !== undefined && value !== null)
    return JSON.parse(JSON.stringify(value));

  return null;
}
