using System.Collections.Generic;
using UnityEngine;

public static class PatternFinder {
    public static List<HitBox> CheckWin(Dictionary<string, HitBox> fields) {
        List<HitBox> match;

        var rows = GameManager.Instance.Rows;
        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < rows; y++) {
                for (int z = 0; z < rows; z++) {
                    // diagonal bottom left, top right
                    match = checkMatch(x, y, 1, -1, fields);

                    // diagonal top left, bottom right
                    if (match == null) {
                        match = checkMatch(x, y, 1, 1, fields);
                    }

                    // horizontal
                    if (match == null) {
                        match = checkMatch(x, y, 1, 0, fields);
                    }

                    // vertical
                    if (match == null) {
                        match = checkMatch(x, y, 0, 1, fields);
                    }

                    if (match != null) {
                        return match;
                    }
                }
            }
        }

        return null;
    }

    private static List<HitBox> checkMatch(int x, int y, int dX, int dY,
        Dictionary<string, HitBox> fields) {
        List<HitBox> hitMatch = new List<HitBox>();
        int type = -1;
        int checkCount = 0;

        var rows = GameManager.Instance.Rows;
        var match = GameManager.Instance.Match;
        while (checkCount < rows && x >= 0 && x < rows &&
               y >= 0 && y < rows) {
            bool found = false;
            var key = $"{x},{y}";
            HitBox marker = fields.ContainsKey(key) ? fields[key] : null;
            if (marker != null && marker.Type != -1) {
                if (type == -1) {
                    type = marker.Type;
                }

                if (marker.Type == type) {
                    hitMatch.Add(marker);
                    found = true;
                }
            }

            if (!found && hitMatch.Count < rows) {
                if (hitMatch.Count >= match) {
                    return hitMatch;
                }

                hitMatch.Clear();
                type = -1;
            }


            x += dX;
            y += dY;
            checkCount++;
        }

        return hitMatch.Count >= match ? hitMatch : null;
    }
}