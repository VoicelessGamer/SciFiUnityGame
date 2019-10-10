using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBuilderFactory {
    public static SectionBuilder getSectionBuilder(string type) {
        switch(type) {
            case "STANDARD":
                return new StandardSectionBuilder();
        }

        return null;
    }
}
